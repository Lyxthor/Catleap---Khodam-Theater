
public class StandTable
{
    public static void Up()
    {
        Schema.Create("stands", delegate (Blueprint table)
        {
            table.Integer("id", 255).PrimaryKey().AutoIncrement();
            table.String("name", 255).NotNull();
            table.String("image_url", 255).NotNull();
            table.Integer("price").NotNull();
            table.Integer("level").NotNull().Default(1);
            table.String("active_state", 255).NotNull().Default("locked");
        });
    }
    public static void Down()
    {
        Schema.Drop("stands");
    }
}
