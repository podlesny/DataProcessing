using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DataProcessing {
	public partial class ReduceDimForm : Form {
		public ReduceDimForm(List<double[]> arr_) {
			InitializeComponent();
			arr = arr_;
			for(int i = 0; i < arr[0].Length; i++) {
				table.Columns.Add(i.ToString(), i.ToString());
			}
			table.Rows.Add(arr.Count);
			for(int i =0; i < arr.Count; i++) {
				for(int j = 0; j < arr[i].Length; j++) {
					table[j, i].Value = arr[i][j];
				}
			}
		}
		List<double[]> arr;
		private void del_Click(object sender, EventArgs e) {
			if(table.SelectedRows.Count == 0) {
				return;
			}
			List<int> index = new List<int>();
			for(int i = 0; i < table.SelectedRows.Count; i++) {
				index.Add(table.SelectedRows[i].Index);
			}
			index.Sort();
			for (int i = index.Count-1; i <= 0; i++) {
				arr.RemoveAt(index[i]);
				table.Rows.RemoveAt(index[i]);
			}
			
		}
	}
}
