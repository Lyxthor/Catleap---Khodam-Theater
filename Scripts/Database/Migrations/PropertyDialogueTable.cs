
public class PropertyDialogueTable
{
    public static void Up()
    {
        Schema.Create("property_dialogues", delegate (Blueprint table)
        {
            table.Integer("id", 255).PrimaryKey().AutoIncrement();
            table.Integer("property_id", 255).NotNull();
            table.String("property_type", 255).NotNull();
            table.String("dialogue_box_url", 255).NotNull();
        });
    }
    public static void Down()
    {
        Schema.Drop("property_dialogues");
    }
}
