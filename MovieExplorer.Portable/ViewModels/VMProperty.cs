namespace MovieExplorer.ViewModels
{
    class VMProperty<T>
    {
        public VMProperty(string propertyName)
        {
            PropertyName = propertyName;
        }

        public string PropertyName { get; }
    }
}
