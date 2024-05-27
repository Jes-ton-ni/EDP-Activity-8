using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net.Http;
using Newtonsoft.Json;


namespace CsharpApi
{
    public partial class Form1 : Form
    {
        private static readonly HttpClient client = new HttpClient();
        public Form1()
        {
            InitializeComponent();
        }

        private async void btnGet_Click(object sender, EventArgs e)
        {
            try
            {
                txtOutput.Clear();
                HttpResponseMessage response = await client.GetAsync("http://localhost/myapi/phpapi/api.php");
                response.EnsureSuccessStatusCode();
                string responseBody = await response.Content.ReadAsStringAsync();
                txtOutput.Text = responseBody;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }

        private async void btnPost_Click(object sender, EventArgs e)
        {
            var userData = new { username = txtUsername.Text, pass = txtPassword.Text, email = txtEmail.Text };
            string json = JsonConvert.SerializeObject(userData);
            HttpContent content = new StringContent(json, Encoding.UTF8, "application/json");

            try
            {
                HttpResponseMessage response = await client.PostAsync("http://localhost/myapi/phpapi/api.php", content);
                string responseBody = await response.Content.ReadAsStringAsync();

                if (response.StatusCode == System.Net.HttpStatusCode.Conflict || responseBody.Contains("Duplicate entry"))
                {
                    // Handle duplicate entry
                    txtOutput.Text = "Error: Duplicate entry. Please use a different username or email.";
                }
                else
                {
                    response.EnsureSuccessStatusCode();
                    txtOutput.Text = responseBody;

                    // Clear text boxes after successful post
                    txtUsername.Text = string.Empty;
                    txtPassword.Text = string.Empty;
                    txtEmail.Text = string.Empty;
                }
            }
            catch (Exception ex)
            {
                txtOutput.Text = "Error: " + ex.Message;
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }
    }
}
