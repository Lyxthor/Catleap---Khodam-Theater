
public class PlayzoneTable
{
    public static void Up()
    {
        Schema.Create("playzone", delegate (Blueprint table)
        {
            table.Integer("id", 255).PrimaryKey().AutoIncrement();
            table.String("name", 255).NotNull();
            table.String("image_url", 255).NotNull();
            table.String("scene_name", 255).NotNull();
        });
    }
    public static void Down()
    {
        Schema.Drop("stands");
    }
}
