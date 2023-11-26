using Borrador3Proyecto.Utils;
using Google.Cloud.Firestore;
using Google.Type;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Grpc.Core.Metadata;

namespace Borrador3Proyecto.Data.Repositories
{
    public class EmotionRepository : IRepository<Emotion>
    {
        private const string COLLECTION_NAME = "Emotion";
        private readonly Connection _connection;

        public EmotionRepository(Connection dbConnection)
        {

            _connection = dbConnection;
        }

        public void Delete(string id)
        {
            try
            {
                MessageLogger.LogWarningMessage($"Deleting...{id}");

                var recordRef = _connection.FirestoreDb.Collection(COLLECTION_NAME).Document(id);
                recordRef.DeleteAsync().ConfigureAwait(false).GetAwaiter().GetResult();

                MessageLogger.LogInformationMessage($"Success Delete...{id}");

            }
            catch (Exception ex)
            {
                MessageLogger.LogErrorMessage(ex);
                throw;
            }
        }

        public List<Emotion> FindAll()
        {
            
            try
            {
                MessageLogger.LogInformationMessage($"FindAll...");

                Query query = _connection.FirestoreDb.Collection(COLLECTION_NAME);
                var querySnapshot = query.GetSnapshotAsync().ConfigureAwait(false).GetAwaiter().GetResult();
                var list = new List<Emotion>();
                foreach (var documentSnapshot in querySnapshot.Documents)
                {
                    if (!documentSnapshot.Exists) continue;
                    var data = documentSnapshot.ConvertTo<FirestoreModels.EmotionFirestoreModel>();
                    if (data == null) continue;
                    data.Id = documentSnapshot.Id;
                    list.Add(MapFirebaseModelToEntity(data));
                }
                MessageLogger.LogInformationMessage($"Success FindAll");
                return list;                           
            }
            catch (Exception ex)
            {
                MessageLogger.LogErrorMessage(ex);
                throw;
            }

        }

        public Emotion FindById(string id)
        {
            try
            {
                MessageLogger.LogInformationMessage($"FindById...{id}");

                var docRef = _connection.FirestoreDb.Collection(COLLECTION_NAME).Document(id);
                var snapshot = docRef.GetSnapshotAsync().ConfigureAwait(false).GetAwaiter().GetResult();
                if (snapshot.Exists)
                {
                    var EmotionModel = snapshot.ConvertTo<FirestoreModels.EmotionFirestoreModel>();
                    EmotionModel.Id = snapshot.Id;
                    MessageLogger.LogInformationMessage($"Success FindById...{id}");
                    return MapFirebaseModelToEntity(EmotionModel);
                }
                MessageLogger.LogWarningMessage("Collection class dosen't exsit");
                return null;

                  }
                catch (Exception ex)
                {
                MessageLogger.LogErrorMessage(ex);
                throw;
                   }
        }


        public Emotion Update(Emotion entity)
        {

            try
            {
                MessageLogger.LogInformationMessage($"update...{entity.Emocion}");

                var recordRef = _connection.FirestoreDb.Collection(COLLECTION_NAME).Document(entity.Id);
                var fbModel = MapEntityToFirestoreModel(entity);
                recordRef.SetAsync(fbModel, SetOptions.MergeAll).ConfigureAwait(false).GetAwaiter().GetResult();

                MessageLogger.LogInformationMessage($"Success update...{entity.Emocion}");
                return entity;

            }
            catch (Exception ex)
            {
                MessageLogger.LogErrorMessage(ex);
                throw;
            }
        }

        public void Insert(Emotion entity)
        {
            try
            {
                MessageLogger.LogInformationMessage($"Insert...{entity.Emocion}");

                var fbModel = MapEntityToFirestoreModel(entity);
                var colRef = _connection.FirestoreDb.Collection(COLLECTION_NAME);
                var doc = colRef.AddAsync(fbModel).ConfigureAwait(false).GetAwaiter().GetResult();

                MessageLogger.LogInformationMessage($"Success Insert...{entity.Emocion}");
            }
            catch (Exception ex)
            {
                MessageLogger.LogErrorMessage(ex);
                throw;
            }
        }

        private FirestoreModels.EmotionFirestoreModel MapEntityToFirestoreModel(Emotion emotion)
        {
            return new FirestoreModels.EmotionFirestoreModel
            {
                Id = emotion.Id,
                Emocion = emotion.Emocion,
               // Autor = emotion.Autor
            };
        }
        private Emotion MapFirebaseModelToEntity(FirestoreModels.EmotionFirestoreModel model)
        {
            return new Emotion(model.Id, model.Emocion/*, model.Autor*/);
        }
    }


}