using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Globalization;
using System.Text.RegularExpressions;

namespace Task
{
    public partial class Form1 : DevExpress.XtraEditors.XtraForm
    {

        DataClassesDataContext db = new DataClassesDataContext();
        string regex = (@"^([0-2][0-9]|(3)[0-1])(.)(((0)[0-9])|((1)[0-2]))(.)\d{4}$");
        IQueryable dataSource;

        public Form1()
        {
            InitializeComponent();
            setLocalSource();
            setGridSource(selectEverythingFromDb());
        }

        private IQueryable selectEverythingFromDb()
        {

            return from data in db.Exports
                   select new Eksport
                   { Nazwa = data.Nazwa,
                     Data = data.Data,
                     Godzina = data.Godzina,
                     Uzytkownik = data.Uzytkownik,
                     Lokal = data.Lokal
                   };
        }
        private void btnApprove_Click(object sender, EventArgs e)
        {
            Match matchFrom = Regex.Match(dEFrom.Text, regex);
            Match matchTo = Regex.Match(dETo.Text, regex);

            if (cbLocal.SelectedIndex == 0)
            {
                MessageBox.Show("Wybierz lokal.");
            }
            
            else if(!matchFrom.Success)
            {
                MessageBox.Show("Wybierz datę od:");
            }
            else if (!matchTo.Success)
            {
                MessageBox.Show("Wybierz datę do:");
            }
            else
            {
                try
                {
                    selectDataFromDb();
                    setGridSource(dataSource);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(String.Format("Wystąpił błąd:", ex));
                }
            }
            
        
        }

        private void setGridSource(IQueryable source)
        {
            gdvReport.DataSource = source;             
        }

        private void setLocalSource()
        {
            var selectedDataLocal = from data in db.Exports select data.Lokal;
            var localItemsList = selectedDataLocal.ToList();
            localItemsList.Insert(0,"Lokal:");

            cbLocal.DataSource = localItemsList;
            
        }
        private void selectDataFromDb()
        {
            dataSource = from data in db.Exports
                         where data.Lokal.Equals(cbLocal.Text) && data.Data.Value >= dEFrom.DateTime && data.Data.Value <= dETo.DateTime
                         select new Eksport
                         {
                             Nazwa = data.Nazwa,
                             Data = data.Data,
                             Godzina = data.Godzina,
                             Uzytkownik = data.Uzytkownik,
                             Lokal = data.Lokal
                         };
        }

       
    }
}
