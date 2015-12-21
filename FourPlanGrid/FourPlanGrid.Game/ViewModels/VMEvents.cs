namespace FourPlanGrid.Game.ViewModels
{
    using System.Windows.Media;

    /// <summary>
    /// Event classes for the event aggregator. We use this to pass messages
    /// between view models
    /// </summary>


    class PlayerColorChangedEvent : Prism.Events.PubSubEvent<PlayerColor> { }
    class PlayerColor
    {
        public Color color;
        public int player;
    }

    /// <summary>
    /// For alerting other view models that a TokenViewModel was created
    /// </summary>
    class TokenViewModelCreatedEvent : Prism.Events.PubSubEvent<TokenViewModel> { }

    /// <summary>
    /// Used to alert other view models that a new game event has been triggered. 
    /// Any game/board state reset.
    /// </summary>
    class NewGameEvent : Prism.Events.PubSubEvent<object> { }


    class TokenPlacedEvent : Prism.Events.PubSubEvent<TokenViewModel> { }

}
