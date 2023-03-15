using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace AutoConector.Asset
{
    public static class ResourceImage
    {
        public static BitmapImage GetIcon(string name)
        {
            Stream stream = ResourceAssembly.GetAssembly().GetManifestResourceStream(ResourceAssembly.GetNamespace() + "Image." + name);

            BitmapImage imge = new BitmapImage();

            imge.BeginInit();

            imge.StreamSource = stream;

            imge.EndInit();

            return imge;
        }
    }
}
