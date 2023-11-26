using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Borrador3Proyecto.Utils
{

    public static class EmotionAppRunner
    {
        public static void Run()
        {
            const string DATA_FILE = "emotions.json";

            string jsonText = File.ReadAllText(DATA_FILE);
            JObject jsonObject = JObject.Parse(jsonText);
            JArray emotionsArray = jsonObject["Sentimientos"] as JArray;

            if (emotionsArray != null)
            {
                List<Emotion> emotions = emotionsArray.ToObject<List<Emotion>>();
                Console.WriteLine("Lista de emociones disponibles:\n");
                for (int i = 0; i < emotions.Count; i++)
                {
                    Console.WriteLine($"{i + 1}. {emotions[i].Emocion}");
                }

               

                Console.Write("\nElige una de las emociones: ");


                int selectedEmotionIndex;
                if (int.TryParse(Console.ReadLine(), out selectedEmotionIndex) && selectedEmotionIndex >= 1 && selectedEmotionIndex <= emotions.Count)
                {
                    Emotion chosenEmotion = emotions[selectedEmotionIndex - 1];
                    Console.WriteLine(" ");
                    Console.WriteLine($"Artistas relacionados con {chosenEmotion.Emocion}:\n");

                    for (int i = 0; i < chosenEmotion.MusicList.Count; i++)
                    {
                        Console.WriteLine($"{i + 1}. {chosenEmotion.MusicList[i].Autor}");
                    }

                    Console.Write("\nElige uno de los artistas: ");

                    int selectedArtistIndex;
                    if (int.TryParse(Console.ReadLine(), out selectedArtistIndex) && selectedArtistIndex >= 1 && selectedArtistIndex <= chosenEmotion.MusicList.Count)
                    {
                        MusicInfo chosenArtist = chosenEmotion.MusicList[selectedArtistIndex - 1];

                        Console.WriteLine(" ");
                        Console.WriteLine("Información sobre la canción:\n");
                        Console.WriteLine($"Álbum: {chosenArtist.Álbum}");
                        Console.WriteLine($"Autor: {chosenArtist.Autor}");
                        Console.WriteLine($"Género: {chosenArtist.Género}");
                        Console.WriteLine($"URL de la Imagen del Álbum: {chosenArtist.URL_de_la_Imagen_del_Álbum}");
                        Console.WriteLine($"URL de la Imagen del Artista: {chosenArtist.URL_de_la_Imagen_del_Artista}");
                        Console.WriteLine($"URL del Video: {chosenArtist.URL_del_Video}");
                        Console.Write("\n¿Estás de acuerdo con esta elección? (Si/No): ");

                        string agreement = Console.ReadLine();

                        if (agreement.Equals("No", StringComparison.OrdinalIgnoreCase))
                        {
                            Run();
                        }
                        else
                        {
                            Console.WriteLine(" ");
                            Console.WriteLine("¡Gracias por usar la aplicación!");
                        }
                    }
                    else
                    {
                        Console.WriteLine(" ");
                        Console.WriteLine("Artista no encontrado. Reiniciando la lista de emociones.");
                        Run();
                    }
                }
                else
                {
                    Console.WriteLine(" ");
                    Console.WriteLine("Selección de emoción no válida. Reiniciando la lista de emociones.");
                    Run();
                }
            }
            else
            {
                Console.WriteLine(" ");
                Console.WriteLine("No se encontró la propiedad 'Sentimientos' en el JSON.");
            }
        }
    }
}
