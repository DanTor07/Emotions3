using Borrador3Proyecto.Data.Repositories;
using Borrador3Proyecto.Data;
using Borrador3Proyecto.Utils;
using System;

namespace Borrador3Proyecto
{
    class Program
    {
        static void Main()
        {
            Console.WriteLine("Lista de emociones disponibles:");
            Console.WriteLine("1. Aburrimiento");
            Console.WriteLine("2. Alegria");
            Console.WriteLine("3. Esperanza");
            Console.WriteLine("4. Ira");
            Console.WriteLine("5. Nostalgico");
            Console.WriteLine("6. Romance");
            Console.WriteLine("7. Tristeza");

            Console.Write("Seleccione una emoción (1-7): ");
            int selectedEmotionIndex;
            if (int.TryParse(Console.ReadLine(), out selectedEmotionIndex) && selectedEmotionIndex >= 1 && selectedEmotionIndex <= 7)
            {
                string selectedEmotion = GetEmotionByIndex(selectedEmotionIndex);

                Connection dbConn = new Connection(selectedEmotion);
                dbConn.InsertEmotionAndAuthor();
                EmotionRepository emotionRepo = new EmotionRepository(dbConn);

                Console.WriteLine("------findAll");

                var all = emotionRepo.FindAll();

                foreach (var item in all)
                {
                    Console.WriteLine($"{item.Id}) {item.Emocion}");
                }

                Console.WriteLine("------Find by id");

                var oneEntity = emotionRepo.FindById(all.First().Id);
                Console.WriteLine($"{oneEntity.Id}{oneEntity.Emocion}");

                Emotion updateEmotion = all.Last();
                //updateEmotion.Emocion = "Nostalgia";

                emotionRepo.Update(updateEmotion);

                // Llama a la función Run en EmotionApp después de la elección inicial de la emoción
                EmotionApp.Run(selectedEmotion);
            }
        }

        private static string GetEmotionByIndex(int index)
        {
            switch (index)
            {
                case 1: return "Aburrimiento";
                case 2: return "Alegria";
                case 3: return "Esperanza";
                case 4: return "Ira";
                case 5: return "Nostalgico";
                case 6: return "Romance";
                case 7: return "Tristeza";
                default: return string.Empty;
            }
        }
    }
}
