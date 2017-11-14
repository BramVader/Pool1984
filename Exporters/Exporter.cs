using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pool1984.Exporters
{
    abstract class Exporter
    {
        public Exporter()
        {
        }

        public abstract string GetFileDialogFilter();

        public abstract void Export(Stream outputStream, Model model);
    }
}
