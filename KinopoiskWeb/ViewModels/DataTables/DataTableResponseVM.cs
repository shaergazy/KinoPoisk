namespace KinopoiskWeb.DataTables
{
    public class DataTablesResponseVM<TEntity>
    where TEntity : class
    {
        public int Draw { get; set; }

        public int RecordsTotal { get; set; }

        public int RecordsFiltered { get; set; }

        public IList<TEntity> Data { get; set; }
    }
}
