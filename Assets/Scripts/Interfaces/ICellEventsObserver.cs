namespace Runtime
{
    public interface ICellEventsObserver
    {
        public void CellClicked(Cell cell);
        public void CellSelected(Cell cell);
    }
}
