using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Hosting;
using System.Web.Mvc;
using Test.Models;

namespace Test.Controllers
{

    public class SentenceController : Controller
    {
        DataContext data;

        public SentenceController()
        {
            data = new DataContext();
        }

        public ActionResult Index()
        {
            IEnumerable<Sentence> sentences = data.Sentences;
            foreach (var sentence in sentences)
            {
                sentence.Text = Reverse(sentence.Text);
            }
            ViewBag.Sentences = sentences;
            return View();
        }

        [HttpGet]
        public void SentenceList()
        {
            IEnumerable<Sentence> sentences = data.Sentences;
            foreach (var sentence in sentences)
            {
                sentence.Text = Reverse(sentence.Text);
            }
            ViewBag.Sentences = sentences;
        }

        [HttpGet]
        public void GetSentences(RequestModel _model)
        {
            if(_model != null && _model.Word!=null)
            {
                var text = ReadTextFile(_model.FileName);
                var sentences = FindSentences(text, _model.Word);
                foreach (var sentence in sentences)
                {
                    sentence.Text = Reverse(sentence.Text);
                    data.Sentences.Add(sentence);
                }
                data.SaveChanges();
            }
        }

        public string ReadTextFile(string fileName)
        {
            string filePath = HostingEnvironment.ApplicationPhysicalPath + "App_Data/" + fileName;
            string data;
            try
            {
                using (StreamReader sr = new StreamReader(filePath))
                {
                    data = sr.ReadToEnd();
                }
            }
            catch 
            {
                data = null;
            }
            return data;
        }

        public List<Sentence> FindSentences(string text, string word)
        {
            var result = new List<Sentence>();
            var sentences = text.Split('.');
            var regex = new Regex(@word, RegexOptions.IgnoreCase);
            for (var i = 0; i < sentences.Length; i++)
            {
                MatchCollection matches = regex.Matches(sentences[i]);

                if (matches.Count>0)
                {
                    var sentence = new Sentence()
                    {
                        Text = sentences[i],
                        Count = matches.Count
                    };
                    result.Add(sentence);
                }
            }
            return result;
        }

        public string Reverse(string sentence)
        {
            var arr = sentence.ToCharArray();
            Array.Reverse(arr);
            return new string(arr);
        }

    }
}