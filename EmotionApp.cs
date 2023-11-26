using Newtonsoft.Json;
using Google.Cloud.Firestore;
using System;
using System.Collections.Generic;
using System.IO;
using Borrador3Proyecto.Data;
using Newtonsoft.Json.Linq;

namespace Borrador3Proyecto
{
    class EmotionApp
    {
        public static void Run(string selectedEmotion)
        {
            const string DATA_FILE = "emotions.json";

            string jsonText = File.ReadAllText(DATA_FILE);
            JObject jsonObject = JObject.Parse(jsonText);
            JArray emotionsArray = jsonObject["Sentimientos"] as JArray;

            if (emotionsArray != null)
            {
                List<Emotion> emotions = emotionsArray.ToObject<List<Emotion>>();
                Console.WriteLine($"Emoción seleccionada: {selectedEmotion}");

                Console.WriteLine(" ");
                Console.WriteLine($"Artistas relacionados con {selectedEmotion}:\n");

                for (int i = 0; i < emotions.Count; i++)
                {
                    if (emotions[i].Emocion == selectedEmotion)
                    {
                        for (int j = 0; j < emotions[i].MusicList.Count; j++)
                        {
                            Console.WriteLine($"{j + 1}. {emotions[i].MusicList[j].Autor}");
                        }
                        break;
                    }
                }

                Console.Write("\nElige uno de los artistas: ");

                int selectedArtistIndex;
                if (int.TryParse(Console.ReadLine(), out selectedArtistIndex))
                {
                    if (selectedArtistIndex >= 1)
                    {
                        // Resta 1 porque la lista se muestra desde 1 pero se indexa desde 0
                        selectedArtistIndex--;

                        if (selectedArtistIndex < emotions.Find(e => e.Emocion == selectedEmotion).MusicList.Count)
                        {
                            MusicInfo chosenArtist = emotions.Find(e => e.Emocion == selectedEmotion).MusicList[selectedArtistIndex];

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
                                Run(selectedEmotion);
                            }
                            else
                            {
                                Console.WriteLine(" ");
                                Console.WriteLine("¡Gracias por usar la aplicación!");
                                Connection dbConn = new Connection(emotions.Find(e => e.Emocion == selectedEmotion).Autor);
                                dbConn.InsertEmotionAndSongInfo(chosenArtist);
                            }
                        }
                        else
                        {
                            Console.WriteLine("Opción no válida.");
                        }
                    }
                }
            }
        }
    }
}