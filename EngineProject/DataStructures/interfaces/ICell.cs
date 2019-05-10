using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EngineProject.DataStructures.interfaces
{
    public interface ICell
    {
        void SetState(bool _state);
        bool GetState();
        int X();
        int Y();
    }
}
