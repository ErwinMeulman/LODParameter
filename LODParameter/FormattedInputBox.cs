using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System;
using System.Windows.Forms;

namespace LODParameter
{
	internal class FormattedInputBox
	{
		private double m_Value = 0.0;

		public TextBox FormTextBox
		{
			get;
		}

		public Units InputUnits
		{
			get;
		}

		public UnitType InputUnitType
		{
			get;
		}

		public double Value
		{
			get
			{
				return m_Value;
			}
			set
			{
				m_Value = value;
				FormTextBox.Text = FormattedValue;
			}
		}

		public string FormattedValue
		{
			get
			{
				return UnitFormatUtils.Format(InputUnits, InputUnitType, Value, false, false);
			}
			set
			{
				double value2 = default(double);
				if (!UnitFormatUtils.TryParse(InputUnits, InputUnitType, value, ref value2))
				{
					throw new FormatException("Failed to parse number from formatted string.");
				}
				Value = value2;
			}
		}

		public FormattedInputBox(TextBox textbox, Units docUnits, UnitType unitType)
		{
			FormTextBox = textbox;
			InputUnits = docUnits;
			InputUnitType = unitType;
			FormTextBox.LostFocus += FormTextBox_LostFocus;
		}

		private void FormTextBox_LostFocus(object sender, EventArgs e)
		{
			try
			{
				FormattedValue = FormTextBox.Text;
			}
			catch (FormatException)
			{
				TaskDialog.Show("Invalid Unit Format", "Could not understand input value. Please try again.");
				FormTextBox.SelectAll();
			}
		}
	}
}
