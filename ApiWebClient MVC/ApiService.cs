using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Reflection.Metadata;
using Newtonsoft.Json;
using System.Diagnostics;
using System.Windows;
using ApiWebClient.Models;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Http;

namespace ApiWebClient
{
    public class ApiService
    {
        private readonly IConfiguration _configuration;


        public ApiService()
        {
            
        }

        public async Task<List<Author>> loadAuthors()
        {
            using var client = new HttpClient();
            var reponse = await client.GetAsync("https://localhost:7008/api/authors");
            if (reponse.IsSuccessStatusCode)
            {
                var authors = await reponse.Content.ReadAsStringAsync();
                var serviceResponse = JsonConvert.DeserializeObject<List<Author>>(authors);
                return serviceResponse;
            }
            else return null;
        }

        public async Task<List<Book>> loadBooks()
        {
            using var client = new HttpClient();
            var response = await client.GetAsync("https://localhost:7008/api/books");
            if (response.IsSuccessStatusCode)
            {

                var books = await response.Content.ReadAsStringAsync();
                var serviceResponse = JsonConvert.DeserializeObject<List<Book>>(books);
                return serviceResponse;
            }
            else return null;
            
        }

        public async Task<Author> loadAuthor(int id)
        {
            using var client = new HttpClient();
            var reponse = await client.GetAsync($"https://localhost:7008/api/authors/{id}");
            if (reponse.IsSuccessStatusCode)
            {
                var authors = await reponse.Content.ReadAsStringAsync();
                await Console.Out.WriteLineAsync(authors);
                var serviceResponse = JsonConvert.DeserializeObject<ServiceResponse<Author>>(authors);
                return serviceResponse.Data;
            }
            else return null;
        }

        public async Task<ServiceResponse<Book>> loadBook(int id)
        {
            using var client = new HttpClient();
            var response = await client.GetAsync($"https://localhost:7008/api/books/{id}");
            if (response.IsSuccessStatusCode)
            {
                var book = await response.Content.ReadAsStringAsync();
                var serviceResponse = JsonConvert.DeserializeObject<ServiceResponse<Book>>(book);
                return serviceResponse;
            }
            else return null;
        }

        public async Task<ServiceResponse<Book>> deleteBook(int id)
        {
            using var client = new HttpClient();
            var response = await client.DeleteAsync($"https://localhost:7008/api/books/{id}");
            return null;
        }

        public async Task<ServiceResponse<Book>> addBook(string Title, Author Author, string Description, string cookie)
        {
            using var client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", cookie);
            var response = await client.GetAsync("https://localhost:7008/api/authors");
            if (response.IsSuccessStatusCode)
            {
                var authors = JsonConvert.DeserializeObject<List<Author>>(await response.Content.ReadAsStringAsync());
                var author = authors.FirstOrDefault(b=>b.Id == Author.Id);
                if(author  == null)
                {
                    return null;
                }
                Book newBook = new Book();
                newBook.Title = Title;
                newBook.Description = Description;
                newBook.Author = author;
                string json = JsonConvert.SerializeObject(newBook);
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                HttpResponseMessage postResponse = await client.PostAsync("https://localhost:7008/api/books", content);
                if (postResponse.IsSuccessStatusCode)
                {
                    return null;
                }

            }
            return null;
        }

        public async Task<ServiceResponse<Book>> updateBook(string Title, Author author, string Description, int id)
        {
            var newBook = (await loadBook(id)).Data;
            using var client = new HttpClient();
           
            newBook.Title = Title;
            newBook.Description = Description;
            newBook.Author = author;
            string json = JsonConvert.SerializeObject(newBook);

            var content = new StringContent(json, Encoding.UTF8, "application/json");
            await Console.Out.WriteLineAsync(json);
            var response = await client.PutAsync($"https://localhost:7008/api/books/{id}", content);
            return null;
        }
    }
}
