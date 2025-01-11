using CSharpEgitimKampi501.Dtos;
using Dapper;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CSharpEgitimKampi501
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        SqlConnection connection = new SqlConnection("Server=DESKTOP-QUL77PV\\SQLEXPRESS;Initial Catalog=EgitimKampi501Db;Integrated Security=True;");
        private async void btnList_Click(object sender, EventArgs e)
        {
            string query = "Select * From TblProduct";
            var values = await connection.QueryAsync<ResultProductDto>(query);
            dataGridView1.DataSource = values;
        }

        private async void btnAdd_Click(object sender, EventArgs e)
        {
            string query = "insert into TblProduct (ProductName,ProductStock,ProductPrice,ProductCategory) values (@productName,@productStock,@productPrice,@productCategory)";
            var parameters = new DynamicParameters();
            parameters.Add("@productName", TxtProductName.Text);
            parameters.Add("@productStock", txtStock.Text);
            parameters.Add("@productPrice", txtPrice.Text);
            parameters.Add("@productCategory", txtCategory.Text);
            await connection.ExecuteAsync(query, parameters);
            MessageBox.Show("Yeni Kitap Ekleme İşlemi Başarılı");
        }

        private async void btnDelete_Click(object sender, EventArgs e)
        {
            string query = "Delete From TblProduct where ProductId=@producID";
            var parameters = new DynamicParameters();
            parameters.Add("@productID", txtProductId.Text);
            await connection.ExecuteAsync(query, parameters);
            MessageBox.Show("Kitap Silme İşlemi Başarılıdır.");
        }

        private async void btnUpdate_Click(object sender, EventArgs e)
        {
            string query = "Update TblProduct Set ProductName=@productName,ProductPrice=@productPrice,ProductStock=@productStock,ProductCategory=@productCategory where ProductId=@productID";
            var parameters = new DynamicParameters();
            parameters.Add("@productName", TxtProductName.Text);
            parameters.Add("@productPrice", txtPrice.Text);
            parameters.Add("@productStock", txtStock.Text);
            parameters.Add("@productCategory", txtCategory.Text);
            parameters.Add("@productID", txtProductId.Text);
            await connection.ExecuteAsync(query, parameters);
            MessageBox.Show("Güncelleme İşlemi Başarıyla Tamamlandı.", "Güncelleme", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private async void Form1_Load(object sender, EventArgs e)
        {
            string query1 = "Select Count(*) From TblProduct";
            var productTotalCount = await connection.QueryFirstOrDefaultAsync<int>(query1);
            lblTotalProductCount.Text = productTotalCount.ToString();


            string query2 = "Select ProductName From TblProduct where ProductPrice=(Select Max(ProductPrice) from TblProduct)";
            var maxPriceProductName = await connection.QueryFirstOrDefaultAsync<string>(query2);
            lblmaxPriceProductName.Text = maxPriceProductName.ToString();


            string query3 = "Select Count(Distinct(ProductCategory)) From TblProduct";
            var distinctProductCount = await connection.QueryFirstOrDefaultAsync<int>(query3);
            lblDistinctCategoryCount.Text = distinctProductCount.ToString();
        }
    }
}
//Data Source=DESKTOP-QUL77PV\SQLEXPRESS;Initial Catalog=EgitimKampi501Db;Integrated Security=True;Encrypt=True;Trust Server Certificate=True