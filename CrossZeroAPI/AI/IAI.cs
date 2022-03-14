namespace CrossZeroAPI {
    public interface IAI : IPlayer {
        ReadOnlyTable GameField { set; }
        Marks Mark { set; }
    }
}
