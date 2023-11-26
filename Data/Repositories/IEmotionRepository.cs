using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Borrador3Proyecto.Data.Repositories
{
    public interface IEmotionRepository
    {
        void Insert(Emotion emotion);
        List<Emotion> FindAll();
        Emotion FindById(string id);
        void Delete(string id);
        Emotion Update(Emotion emotion);
    }
}
