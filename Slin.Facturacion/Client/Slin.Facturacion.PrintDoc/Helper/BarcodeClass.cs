using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Drawing;
using System.Drawing.Imaging;
using System.Drawing.Drawing2D;

using iTextSharp.text.pdf;
//using iTextSharp.text.pdf.BarcodeCodabar;

namespace Slin.Facturacion.PrintDoc.Helper
{
    class BarcodeClass
    {
        //public object codigo128(string _code, int Height = 0)
        public bool codigo128(string _code, int Height, string PathSaveCodeBar)
        {
            Image bmT = default(Image);
            bool result = false;

            BarcodeCodabar barcode = new BarcodeCodabar();
            //barcode.StartStopText = false;
            if (Height != 0)
            {
                barcode.BarHeight = Height;
            }
            barcode.Code = _code;
            try
            {
                System.Drawing.Bitmap bm = new System.Drawing.Bitmap(barcode.CreateDrawingImage(Color.Black, Color.White));
                bmT = default(Image);
                bmT = new Bitmap(bm.Width, bm.Height + 14);
                Graphics g = Graphics.FromImage(bmT);
                g.FillRectangle(new SolidBrush(Color.White), 0, 0, bm.Width, bm.Height + 14);

                Font pintarTexto = new Font("Arial", 8);
                SolidBrush brocha = new SolidBrush(Color.Black);

                SizeF stringSize = new SizeF();
                stringSize = g.MeasureString(_code, pintarTexto);
                float centrox = (bm.Width - stringSize.Width) / 2;
                float x = centrox;
                float y = bm.Height;

                StringFormat drawformat = new StringFormat();
                drawformat.FormatFlags = StringFormatFlags.NoWrap;
                g.DrawImage(bm, 0, 0);

                string ncode = _code.Substring(1, _code.Length - 2);
                g.DrawString(ncode, pintarTexto, brocha, x, y, drawformat);

                bm.Save(PathSaveCodeBar);

                result = true;
            }
            catch (Exception ex)
            {
                //throw new Exception("Error al generar el codigo" + ex.ToString);
                result = false;
            }
            return result;
        }
    }
}















