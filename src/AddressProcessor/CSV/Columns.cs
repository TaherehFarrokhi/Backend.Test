namespace AddressProcessing.CSV
{
    public class Columns
    {
        public string Column1 { get;  }
        public string Column2 { get; }

        public Columns(string column1, string column2)
        {
            Column1 = column1;
            Column2 = column2;
        }
    }
}