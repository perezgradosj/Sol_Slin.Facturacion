using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.Drawing;
using System.Drawing.Printing;
using System.Collections;
using Slin.Facturacion.Common;
using System.Drawing.Imaging;
using System.Windows.Forms;
using System.Diagnostics;
using System.IO;


namespace Slin.Facturacion.Electronica.Web.Helper.Print
{
    public class Tickets
    {



        #region OTHERS

        string ticket = string.Empty;
        int max = Constantes.ValorCero;
        int cort = Constantes.ValorCero;
        string parte1 = string.Empty;

        public void TextoCentro(string par1)
        {
            ticket = "";
            max = par1.Length;
            if (max > 36)                                 // **********
            {
                cort = max - 36;
                parte1 = par1.Remove(36, cort);          // si es mayor que 36 caracteres, lo corta
            }
            else { parte1 = par1; }                      // **********
            max = (int)(36 - parte1.Length) / 2;         // saca la cantidad de espacios libres y divide entre dos
            for (int i = 0; i < max; i++)                // **********
            {
                ticket += " ";                           // Agrega espacios antes del texto a centrar
            }                                            // **********
            ticket += parte1 + "\n";

            TextoAlCentro(ticket);
        }

        public void TextoAlCentro(string parte1)
        {
            headerLines.Add(parte1);
        }

        public void TextoAlCentroFooter(string line)
        {
            footerLines.Add(line);
        }

        public void TextoCentroFooter(string par1)
        {
            ticket = "";
            max = par1.Length;
            if (max > 36)                                 // **********
            {
                cort = max - 36;
                parte1 = par1.Remove(36, cort);          // si es mayor que 36 caracteres, lo corta
            }
            else { parte1 = par1; }                      // **********
            max = (int)(36 - parte1.Length) / 2;         // saca la cantidad de espacios libres y divide entre dos
            for (int i = 0; i < max; i++)                // **********
            {
                ticket += " ";                           // Agrega espacios antes del texto a centrar
            }                                            // **********
            ticket += parte1 + "\n";

            TextoAlCentroFooter(ticket);
        }
        #endregion




        ArrayList headerLines = new ArrayList();
        ArrayList subHeaderLines = new ArrayList();
        ArrayList items = new ArrayList();
        ArrayList totales = new ArrayList();
        ArrayList footerLines = new ArrayList();

        private Image headerImage = null;

        private Image footerImage = null;

        int count = 0;

        //int maxChar = 40;
        int maxChar = 36;
        int maxCharDescription = 20;
        //int maxCharDescription = 20;

        int imageHeight = 0;

        float leftMargin = 0;
        float topMargin = 3;

        string fontName = "Lucida Console";
        //string fontName = "Calibri Light";
        //string fontName = "Arial Narrow";
        //string fontName = "Calibri";
        //string fontName = "Lucida Sans Unicode";



        //int fontSize = 10;
        int fontSize = 9;

        Font printFont = null;
        SolidBrush myBrush = new SolidBrush(Color.Black);

        Graphics gfx = null;

        string line = null;

        public void Ticket()
        {

        }

        public Image HeaderImage
        {
            get { return headerImage; }
            set { if (headerImage != value) headerImage = value; }
        }

        public int MaxChar
        {
            get { return maxChar; }
            set { if (value != maxChar) maxChar = value; }
        }

        public int MaxCharDescription
        {
            get { return maxCharDescription; }
            set
            {
                if (value != maxCharDescription) maxCharDescription
          = value;
            }
        }

        public int FontSize
        {
            get { return fontSize; }
            set { if (value != fontSize) fontSize = value; }
        }

        public string FontName
        {
            get { return fontName; }
            set { if (value != fontName) fontName = value; }
        }

        public void AddHeaderLine(string line)
        {
            headerLines.Add(line);
        }

        public void AddSubHeaderLine(string line)
        {
            subHeaderLines.Add(line);
        }

        public void AddItem(string cantidad, string espacio, string item, string precio, string price)
        {
            OrderItem newItem = new OrderItem('?');
            items.Add(newItem.GenerateItem(cantidad, espacio, item, precio, price));
        }

        //public void AddItem(string cantidad, string item, string price)
        //{
        //    OrderItem newItem = new OrderItem('?');
        //    items.Add(newItem.GenerateItem(cantidad, item, price));
        //}

        public void AddTotal(string name, string price)
        {
            OrderTotal newTotal = new OrderTotal('?');
            totales.Add(newTotal.GenerateTotal(name, price));
        }

        public Image FooterImage
        {
            get { return footerImage; }
            set { if (footerImage != value) footerImage = value; }
        }

        public void AddFooterLine(string line)
        {
            footerLines.Add(line);
        }

        private string AlignRightText(int lenght)
        {
            string espacios = "";
            int spaces = maxChar - lenght;
            for (int x = 0; x < spaces; x++)
                espacios += " ";
            return espacios;
        }

        private string DottedLine()
        {
            string dotted = "";
            for (int x = 0; x < maxChar; x++)
                dotted += "=";
            return dotted;
        }

        public bool PrinterExists(string impresora)
        {
            foreach (String strPrinter in
            PrinterSettings.InstalledPrinters)
            {
                if (impresora == strPrinter)
                    return true;
            }
            return false;
        }
        public void PrintTicket(string impresora)
        {
            //method one
            //printFont = new Font(fontName, fontSize, FontStyle.Regular);
            //PrintDocument pr = new PrintDocument();
            //pr.PrinterSettings = new PrinterSettings();
            //pr.PrintPage += new PrintPageEventHandler(pr_PrintPage);
            //PrintDialog printdialog = new PrintDialog();
            //printdialog.PrinterSettings = new PrinterSettings();
            //printdialog.Document = pr;
            //pr.PrinterSettings.PrinterName = printdialog.PrinterSettings.PrinterName;
            //DialogResult result = printdialog.ShowDialog();
            //if (result == DialogResult.OK)
            //{
            //    pr.DefaultPageSettings.Landscape = false;
            //    pr.Print();
            //}

            if (impresora == string.Empty || impresora == null || impresora == Constantes.ValorSeleccione)
            {
                //method one
                printFont = new Font(fontName, fontSize, FontStyle.Regular);
                PrintDocument pr = new PrintDocument();
                pr.PrinterSettings = new PrinterSettings();
                pr.PrintPage += new PrintPageEventHandler(pr_PrintPage);
                PrintDialog printdialog = new PrintDialog();
                printdialog.PrinterSettings = new PrinterSettings();
                printdialog.Document = pr;
                pr.PrinterSettings.PrinterName = printdialog.PrinterSettings.PrinterName;
                DialogResult result = printdialog.ShowDialog();
                if (result == DialogResult.OK)
                {
                    pr.DefaultPageSettings.Landscape = false;
                    pr.Print();
                }
            }
            else
            {
                //method two
                printFont = new Font(fontName, fontSize, FontStyle.Regular);
                PrintDocument pr = new PrintDocument();
                pr.PrinterSettings.PrinterName = impresora;
                pr.PrintPage += new PrintPageEventHandler(pr_PrintPage);
                pr.Print();
            }


            //method two
            //printFont = new Font(fontName, fontSize, FontStyle.Regular);
            //PrintDocument pr = new PrintDocument();
            //pr.PrinterSettings.PrinterName = impresora;
            //pr.PrintPage += new PrintPageEventHandler(pr_PrintPage);
            //pr.Print();



            var fileImg = Path.Combine(HttpContext.Current.Server.MapPath("../../DocumentoXML/CodigoQR.bmp"));
            if (System.IO.File.Exists(fileImg))
                System.IO.File.Delete(fileImg);

        }

        private void pr_PrintPage(object sender,
        System.Drawing.Printing.PrintPageEventArgs e)
        {
            e.Graphics.PageUnit = GraphicsUnit.Millimeter;
            gfx = e.Graphics;
            DrawImage();
            DrawHeader();
            DrawSubHeader();
            DrawItems();
            DrawTotales();
            DrawImageFooter();
            DrawFooter();
            if (headerImage != null)
            {
                HeaderImage.Dispose();
                headerImage.Dispose();
            }
        }

        private float YPosition()
        {
            return topMargin + (count * printFont.GetHeight(gfx) +
            imageHeight);
        }

        private void DrawImage()
        {
            if (headerImage != null)
            {
                try
                {
                    gfx.DrawImage(headerImage, new Point((int)
                    leftMargin, (int)YPosition()));
                    double height = ((double)headerImage.Height / 58)
                    * 15;
                    imageHeight = (int)Math.Round(height) + 3;
                }
                catch (Exception)
                {
                }
            }
        }

        private void DrawHeader()
        {
            foreach (string header in headerLines)
            {
                if (header.Length > maxChar)
                {
                    int currentChar = 0;
                    int headerLenght = header.Length;

                    while (headerLenght > maxChar)
                    {
                        line = header.Substring(currentChar, maxChar);
                        gfx.DrawString(line, printFont, myBrush,
                        leftMargin, YPosition(), new StringFormat());

                        count++;
                        currentChar += maxChar;
                        headerLenght -= maxChar;
                    }
                    line = header;
                    gfx.DrawString(line.Substring(currentChar,
                    line.Length - currentChar), printFont, myBrush, leftMargin, YPosition
                    (), new StringFormat());
                    count++;
                }
                else
                {
                    line = header;
                    gfx.DrawString(line, printFont, myBrush,
                    leftMargin, YPosition(), new StringFormat());

                    count++;
                }
            }
            //DrawEspacio(); Descomentar Espacio Cabecera
        }

        private void DrawSubHeader()
        {
            foreach (string subHeader in subHeaderLines)
            {
                if (subHeader.Length > maxChar)
                {
                    int currentChar = 0;
                    int subHeaderLenght = subHeader.Length;

                    while (subHeaderLenght > maxChar)
                    {
                        line = subHeader;
                        gfx.DrawString(line.Substring(currentChar,
                        maxChar), printFont, myBrush, leftMargin, YPosition(), new StringFormat
                        ());

                        count++;
                        currentChar += maxChar;
                        subHeaderLenght -= maxChar;
                    }
                    line = subHeader;
                    gfx.DrawString(line.Substring(currentChar,
                    line.Length - currentChar), printFont, myBrush, leftMargin, YPosition
                    (), new StringFormat());
                    count++;
                }
                else
                {
                    line = subHeader;

                    gfx.DrawString(line, printFont, myBrush,
                    leftMargin, YPosition(), new StringFormat());

                    count++;

                    line = DottedLine();

                    gfx.DrawString(line, printFont, myBrush,
                    leftMargin, YPosition(), new StringFormat());

                    count++;
                }
            }
            DrawEspacio();  //Comentar Espacio  Linea
        }

        private void DrawItems()
        {
            OrderItem ordIt = new OrderItem('?');

            //gfx.DrawString(string.Empty, printFont, myBrush, leftMargin, YPosition(), new StringFormat());
            gfx.DrawString("Cant   Descripción   P. Unit Importe", printFont, myBrush, leftMargin, YPosition(), new StringFormat());
            count++;
            DrawEspacio();

            foreach (string item in items)
            {
                line = ordIt.GetItemCantidad(item);

                gfx.DrawString(line, printFont, myBrush, leftMargin,
                YPosition(), new StringFormat());

                line = ordIt.GetItemPrice(item);
                line = AlignRightText(line.Length) + line;

                gfx.DrawString(line, printFont, myBrush, leftMargin,
                YPosition(), new StringFormat());

                string name = ordIt.GetItemName(item);

                //leftMargin = 11;
                if (name.Length > maxCharDescription)
                {
                    int currentChar = 0;
                    int itemLenght = name.Length;

                    while (itemLenght > maxCharDescription)
                    {
                        line = ordIt.GetItemName(item);
                        gfx.DrawString(" " + line.Substring
                        (currentChar, maxCharDescription), printFont, myBrush, leftMargin,
                        YPosition(), new StringFormat());

                        count++;
                        currentChar += maxCharDescription;
                        itemLenght -= maxCharDescription;
                    }

                    line = ordIt.GetItemName(item);
                    gfx.DrawString(" " + line.Substring
                    (currentChar, line.Length - currentChar), printFont, myBrush,
                    leftMargin, YPosition(), new StringFormat());
                    count++;
                }
                else
                {
                    gfx.DrawString(" " + ordIt.GetItemName(item),
                    printFont, myBrush, leftMargin, YPosition(), new StringFormat());

                    count++;
                }
            }

            leftMargin = 0;
            DrawEspacio();
            line = DottedLine();

            gfx.DrawString(line, printFont, myBrush, leftMargin,
            YPosition(), new StringFormat());

            count++;
            DrawEspacio();
        }

        private void DrawTotales()
        {
            OrderTotal ordTot = new OrderTotal('?');

            foreach (string total in totales)
            {
                line = ordTot.GetTotalCantidad(total);
                line = AlignRightText(line.Length) + line;

                gfx.DrawString(line, printFont, myBrush, leftMargin,
                YPosition(), new StringFormat());
                leftMargin = 0;

                line = " " + ordTot.GetTotalName(total);
                gfx.DrawString(line, printFont, myBrush, leftMargin,
                YPosition(), new StringFormat());
                count++;
            }
            leftMargin = 0;
            DrawEspacio();
            //DrawEspacio(); // descomentar espacio
        }


        private void DrawImageFooter()
        {
            if (footerImage != null)
            {
                try
                {
                    gfx.DrawImage(footerImage, new Point((int)
                    leftMargin + 5, (int)YPosition()));
                    double height = ((double)footerImage.Height / 20)
                    * 7;
                    imageHeight = (int)Math.Round(height) - 10; //espacio debajo del codigo de barra
                }
                catch (Exception)
                {
                }
            }
        }

        private void DrawFooter()
        {
            foreach (string footer in footerLines)
            {
                if (footer.Length > maxChar)
                {
                    int currentChar = 0;
                    int footerLenght = footer.Length;

                    while (footerLenght > maxChar)
                    {
                        line = footer;
                        gfx.DrawString(line.Substring(currentChar,
                        maxChar), printFont, myBrush, leftMargin, YPosition(), new StringFormat
                        ());

                        count++;
                        currentChar += maxChar;
                        footerLenght -= maxChar;
                    }
                    line = footer;
                    gfx.DrawString(line.Substring(currentChar,
                    line.Length - currentChar), printFont, myBrush, leftMargin, YPosition
                    (), new StringFormat());
                    count++;
                }
                else
                {
                    line = footer;
                    gfx.DrawString(line, printFont, myBrush,
                    leftMargin, YPosition(), new StringFormat());

                    count++;
                }
            }
            leftMargin = 0;
            DrawEspacio();
        }

        private void DrawEspacio()
        {
            line = "";

            gfx.DrawString(line, printFont, myBrush, leftMargin,
            YPosition(), new StringFormat());

            count++;
        }



        #region MY METHOD

        #endregion
    }

    public class OrderItem
    {
        char[] delimitador = new char[] { '?' };

        public OrderItem(char delimit)
        {
            delimitador = new char[] { delimit };
        }

        public string GetItemCantidad(string orderItem)
        {
            string[] delimitado = orderItem.Split(delimitador);
            return delimitado[0];
        }

        public string GetItemName(string orderItem)
        {
            string[] delimitado = orderItem.Split(delimitador);
            return delimitado[1];
        }

        public string GetItemPrice(string orderItem)
        {
            string[] delimitado = orderItem.Split(delimitador);
            return delimitado[2];
        }

        public string GenerateItem(string cantidad, string espacio, string itemName, string precio, string price)
        {
            return cantidad + delimitador[0] + espacio + itemName + delimitador[0] + precio + price;
        }

        //public string GenerateItem(string cantidad, string itemName,
        //string price)
        //{
        //    return cantidad + delimitador[0] + itemName + delimitador
        //    [0] + price;
        //}
    }

    public class OrderTotal
    {
        char[] delimitador = new char[] { '?' };

        public OrderTotal(char delimit)
        {
            delimitador = new char[] { delimit };
        }

        public string GetTotalName(string totalItem)
        {
            string[] delimitado = totalItem.Split(delimitador);
            return delimitado[0];
        }

        public string GetTotalCantidad(string totalItem)
        {
            string[] delimitado = totalItem.Split(delimitador);
            return delimitado[1];
        }

        public string GenerateTotal(string totalName, string price)
        {
            return totalName + delimitador[0] + price;
        }
    }
}