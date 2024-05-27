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
            // Check if any of the textboxes are empty
            if (string.IsNullOrWhiteSpace(txtUsername.Text) || string.IsNullOrWhiteSpace(txtPassword.Text) || string.IsNullOrWhiteSpace(txtEmail.Text))
            {
                txtOutput.Text = "Error: Username, password, and email cannot be empty.";
                return;
            }

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

        private async void getHobby_Click(object sender, EventArgs e)
        {
            try
            {
                hobbyOutput.Clear();
                HttpResponseMessage response = await client.GetAsync("http://localhost/myapi/phpapi/hobbyapi.php");
                response.EnsureSuccessStatusCode();
                string responseBody = await response.Content.ReadAsStringAsync();
                hobbyOutput.Text = responseBody;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }

        private async void postHobby_Click(object sender, EventArgs e)
        {
            // Check if any of the textboxes are empty
            if (string.IsNullOrWhiteSpace(userIDTextBox.Text) || string.IsNullOrWhiteSpace(hobbyTextBox.Text) || string.IsNullOrWhiteSpace(commentTextBox.Text))
            {
                hobbyOutput.Text = "Error: User ID, hobby, and comment cannot be empty.";
                return;
            }

            var userHobby = new { user_id = userIDTextBox.Text, hobby = hobbyTextBox.Text, comment = commentTextBox.Text };
            string json = JsonConvert.SerializeObject(userHobby);
            HttpContent content = new StringContent(json, Encoding.UTF8, "application/json");

            try
            {
                HttpResponseMessage response = await client.PostAsync("http://localhost/myapi/phpapi/hobbyapi.php", content);
                string responseBody = await response.Content.ReadAsStringAsync();

                response.EnsureSuccessStatusCode();
                hobbyOutput.Text = responseBody;

                // Clear text boxes after successful post
                userIDTextBox.Text = string.Empty;
                hobbyTextBox.Text = string.Empty;
                commentTextBox.Text = string.Empty;
            }
            catch (Exception ex)
            {
                hobbyOutput.Text = "Error: " + ex.Message;
            }
        }
    }
}
