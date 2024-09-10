

public class StandDialogueTable
{
    public static void Up()
    {
        Schema.Create("stand_dialogues", delegate (Blueprint table)
        {
            table.Integer("id", 255).PrimaryKey().AutoIncrement();
            table.String("bubble_text", 255).NotNull();
            table.Integer("stand_id", 255).NotNull();
        });
    }
    public static void Down()
    {
        Schema.Drop("stands");
    }
}
