using System.Reflection;

namespace FrmProject.DataStructures
{
    public static class ViewHelper
    {
        public static void ApplyBaseStyle(Control control)
        {
            control.BackColor = Color.White;
            
            // Set DoubleBuffered to true via reflection (protected property)
            try
            {
                PropertyInfo? pi = typeof(Control).GetProperty("DoubleBuffered", BindingFlags.Instance | BindingFlags.NonPublic);
                pi?.SetValue(control, true, null);
            }
            catch
            {
                // Silently ignore reflection exceptions
            }

            void OnLoad()
            {
                if (System.ComponentModel.LicenseManager.UsageMode == System.ComponentModel.LicenseUsageMode.Designtime)
                    return;

                EnableDoubleBufferingForGrids(control);
                EnableAutoCompleteForComboBoxes(control);
            }

            if (control is UserControl uc)
            {
                uc.Load += (s, e) => OnLoad();
            }
            else if (control is Form frm)
            {
                frm.Load += (s, e) => OnLoad();
            }
        }

        private static void EnableDoubleBufferingForGrids(Control parent)
        {
            foreach (Control ctrl in parent.Controls)
            {
                if (ctrl is DataGridView dgv)
                {
                    try
                    {
                        PropertyInfo? pi = typeof(DataGridView).GetProperty("DoubleBuffered", BindingFlags.Instance | BindingFlags.NonPublic);
                        pi?.SetValue(dgv, true, null);
                    }
                    catch
                    {
                        // Silently ignore
                    }
                }

                if (ctrl.HasChildren)
                    EnableDoubleBufferingForGrids(ctrl);
            }
        }

        private static void EnableAutoCompleteForComboBoxes(Control parent)
        {
            foreach (Control ctrl in parent.Controls)
            {
                if (ctrl is ComboBox cb)
                {
                    cb.AutoCompleteSource = AutoCompleteSource.ListItems;
                    cb.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
                }

                if (ctrl.HasChildren)
                    EnableAutoCompleteForComboBoxes(ctrl);
            }
        }
    }
}
