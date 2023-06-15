using MEC;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using Utils.NonAllocLINQ;

namespace TFP_AutoEvent
{
    internal class EventManager
    {
        public static Stopwatch eventCountdownStopwatch = new Stopwatch();

        public static string eventAssembliesPath = Path.Combine(Exiled.API.Features.Paths.Configs, "TFP-AutoEvents", "EventAssemblies");

        public static List<IEvent> loadedEvents = new List<IEvent>();

        public static string EventName
        {
            get
            {
                if (activeEvent is null)
                    return "<color=red>Ивент не запущен</color>";

                return activeEvent.DisplayName;
            }
        }

        public static string ShortDescription
        {
            get
            {
                if (activeEvent is null)
                    return "<color=red>Ивент не запущен</color>";

                return activeEvent.ShortRulesDescription;
            }
        }

        public static string LongRules
        {
            get
            {
                if (activeEvent is null)
                    return "<color=red>Ивент не запущен</color>";

                return $"Ивент {activeEvent.DisplayName}:\n\n" + activeEvent.ExpandedRules;
            }
        }

        private static IEvent activeEvent;

        public static void Init()
        {
            eventCountdownStopwatch = new Stopwatch();
            eventCountdownStopwatch.Reset();

            Directory.CreateDirectory(eventAssembliesPath); //creates if not present

            ReloadEvents();
        }

        public static void ReloadEvents()
        {
            loadedEvents.Clear();
            loadedEvents.AddRange(GetInternalEvents());
            loadedEvents.AddRange(GetExternalEvents());
        }

        private static void EventCleanup()
        {
            try
            {
                activeEvent.Ended -= EventCleanup;
            }
            catch (NullReferenceException) { }
            activeEvent = null;
        }

        public static IEnumerator<float> LaunchCoroutine(IEvent ev)
        {

            activeEvent = ev;
            int secs = 15;
            ev.PreLaunch();

            while (secs > 0)
            {
                string secondsWord = "";
                if (secs == 1)
                    secondsWord = "секунда";
                if (secs > 1 && secs < 5)
                    secondsWord = "секунды";
                if (secs >= 5 && secs < 21)
                    secondsWord = "секунд";
                else
                {
                    int rem10 = secs % 10;
                    if (rem10 == 1)
                        secondsWord = "секунда";
                    else if (rem10 > 1 && rem10 < 5)
                        secondsWord = "секунды";
                    else
                        secondsWord = "секунд";
                }
                foreach (var player in Exiled.API.Features.Player.List)
                {
                    // TODO: Add colors and stuff
                    player.ShowHint($"Выбран ивент: {EventName}, до начала {secs} {secondsWord}\nИвент вкратце: {ShortDescription}\n\nПодробные правила можно узнать через команду .правила в консоли на [ё].", 2);
                }
                yield return Timing.WaitForSeconds(1f);
                secs -= 1;
            }

            ev.Engage();
        }

        public static void LaunchEvent(IEvent ev)
        {
            if (activeEvent is null)
            {
                Timing.RunCoroutine(LaunchCoroutine(ev));
            }
            else
            {
                throw new Exception("Event is already underway");
            }
        }

        public static void ForciblyStopEvent()
        {
            if (!(activeEvent is null))
            {
                try
                {
                    activeEvent.DisEngage();
                    activeEvent.Ended -= EventCleanup;
                    activeEvent = null;
                }
                catch (NullReferenceException)
                {
                    Exiled.API.Features.Log.Warn("Umm, the event was presumably nulled already. This is not good, maybe the event is over and stuff, but this is odd. Presuming event is already dead and nulling it.");
                    activeEvent = null;
                }
            }
            else
            {
                throw new Exception("Event is not running");
            }
        }

        public static IEvent PickRandomWeightedEvent()
        {
            Dictionary<IEvent, int> eventWeightsSelector = new Dictionary<IEvent, int>();

            int totalWeighting = 0;

            foreach (var ev in loadedEvents)
            {
                string reason;
                bool OK = ev.LaunchCheck(out reason);
                if (!ev.LowPlayerEvent && OK)
                {
                    eventWeightsSelector.Add(ev, ev.EventWeighting);
                    totalWeighting += ev.EventWeighting;
                }
            }

            if (eventWeightsSelector.Count == 0)
            {
                throw new Exception("There are no loaded/avaliable normal events.");
            }

            var rnd = new System.Random();
            int sel = rnd.Next(1, totalWeighting + 1);

            eventWeightsSelector.ForEach(pair => pair = new KeyValuePair<IEvent, int>(pair.Key, pair.Value - sel));

            IEvent winning = eventWeightsSelector.First().Key;
            foreach (var pair in eventWeightsSelector)
            {
                sel -= pair.Value;
                if (sel <= 0)
                {
                    winning = pair.Key;
                    break;
                }
            }

            return winning;
        }

        public static IEvent PickRandomWeightedLowPlayerEvent()
        {
            Dictionary<IEvent, int> eventWeightsSelector = new Dictionary<IEvent, int>();

            int totalWeighting = 0;

            foreach (var ev in loadedEvents)
            {
                string reason;
                bool OK = ev.LaunchCheck(out reason);
                if (ev.LowPlayerEvent && OK)
                {
                    eventWeightsSelector.Add(ev, ev.EventWeighting);
                    totalWeighting += ev.EventWeighting;
                }
            }

            if (eventWeightsSelector.Count == 0)
            {
                throw new Exception("There are no loaded/avaliable low player events.");
            }

            var rnd = new System.Random();
            int sel = rnd.Next(1, totalWeighting + 1);

            eventWeightsSelector.ForEach(pair => pair = new KeyValuePair<IEvent, int>(pair.Key, pair.Value - sel));

            IEvent winning = eventWeightsSelector.First().Key;
            foreach (var pair in eventWeightsSelector)
            {
                sel -= pair.Value;
                if (sel <= 0)
                {
                    winning = pair.Key;
                    break;
                }
            }

            return winning;
        }

        private static List<IEvent> GetExternalEvents()
        {
            var files = Directory.GetFiles(eventAssembliesPath).Where(fl => fl.EndsWith(".dll"));

            List<IEvent> eventsExternal = new List<IEvent>();

            foreach (var assemblyString in files)
            {
                var asm = Assembly.LoadFile(assemblyString);
                var types = asm.GetTypes().Where(type => type.GetInterface(nameof(IEvent)) == typeof(IEvent));
                foreach (var type in types)
                    eventsExternal.Add(Activator.CreateInstance(type) as IEvent); //again, checked before.
            }

            return eventsExternal;
        }

        private static List<IEvent> GetInternalEvents()
        {
            var types = Assembly.GetExecutingAssembly().GetTypes().Where(pred => pred.GetInterface(nameof(IEvent)) == typeof(IEvent));
            List<IEvent> events = new List<IEvent>();

            foreach (var type in types)
                events.Add(Activator.CreateInstance(type) as IEvent); //unsafe, but we have checked them before on line 1 in this method

            return events;
        }

        public static void DeInit()
        {
            eventCountdownStopwatch = null;

            loadedEvents.Clear();
        }
    }
}
