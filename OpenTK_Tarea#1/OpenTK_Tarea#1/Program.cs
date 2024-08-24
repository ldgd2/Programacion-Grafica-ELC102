namespace OpenTK_Tarea_1
{
    class Program
    {
        static void Main()
        {
            using (var game = new Simple3DGame(800, 600, "Tarea 1 - Letra T"))
            {
                game.Run();
            }
        }
    }
}
