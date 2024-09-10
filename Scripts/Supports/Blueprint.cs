
public class Blueprint
{
    private string sql;
    private string cols;
    public Blueprint(string tableName)
    {
        sql = "CREATE TABLE IF NOT EXISTS " + tableName + " (";
    }
    public Blueprint Integer(string col, int len = 255)
    {
        cols += ", " + col + " INTEGER ";
        return this;
    }
    public Blueprint Float(string col)
    {
        cols += ", " + col + " REAL ";
        return this;
    }
    public Blueprint String(string col, int len)
    {
        cols += ", " + col + " TEXT ";
        return this;
    }
    public Blueprint Null()
    {
        cols += "NULL ";
        return this;
    }
    public Blueprint NotNull()
    {
        cols += "NOT NULL ";
        return this;
    }
    public Blueprint PrimaryKey()
    {
        cols += "PRIMARY KEY ";
        return this;
    }
    public Blueprint AutoIncrement()
    {
        cols += "AUTOINCREMENT ";
        return this;
    }
    public Blueprint Default(object value)
    {
        cols += "DEFAULT " + value.ToString() + " ";
        return this;
    }
    public string Query()
    {
        cols = cols.Substring(1);
        sql += cols + ")";
        return sql;
        //SqliteCommand command = new SqliteCommand(sql);
    }
}