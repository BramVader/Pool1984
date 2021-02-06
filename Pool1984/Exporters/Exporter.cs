using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ajv.Pool1984.Exporters
{
    abstract class Exporter
    {
        public Exporter()
        {
        }

        public abstract string GetFileDialogFilter();

        public abstract string GetDefaultLocation();

        public abstract void Export(Stream outputStream, Model model);
    }
}
