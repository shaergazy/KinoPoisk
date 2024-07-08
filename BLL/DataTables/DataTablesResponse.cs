namespace BLL.DataTables
{
    public class DataTablesResponse <TEntity>
        where TEntity : class
    {
        public int Draw { get; set; }

        public int RecordsTotal { get; set; }

        public int RecordsFiltered { get; set; }

        public IList<TEntity> Data { get; set; }

        //public DataTablesResponse(int draw, int recordsTotal, int recordsFiltered, IList<TEntity> data)
        //{
        //    Draw = draw;
        //    RecordsTotal = recordsTotal;
        //    RecordsFiltered = recordsFiltered;
        //    Data = data;
        //}
    }
}
