namespace OpenTK_Tarea_3
{
    class Program
    {
        static void Main()
        {
            using (var game = new Game(800, 600, "Tarea 3"))
            {
                game.Run();
            }
        }
    }
}
