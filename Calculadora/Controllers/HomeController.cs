using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Calculadora.Models;

namespace Calculadora.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            //inicializar valor
            ViewBag.Visor = "0";
            return View();
        }
        [HttpPost]
        public IActionResult Index(string limpaVisor,string visor,string bt,string primeiroValor,string operador)
        {
            //filtrar o conteudo dobt
            switch (bt)
            {
                case "1":
                case "2":
                case "3":
                case "4":
                case "5":
                case "6":
                case "7":
                case "8":
                case "9":
                case "0":
                    //processar algarismos
                    if (visor == "0"||limpaVisor=="true") visor = bt;
                    else visor = visor + bt;

                    ViewBag.PrimeiroOperador = primeiroValor;
                    ViewBag.Operador = operador;
                    break;
                case "+/-" :
                    visor = Convert.ToDouble(visor)* -1 + "";
                    ViewBag.PrimeiroOperador = primeiroValor;
                    ViewBag.Operador = operador;
                    break;
                case ",":
                    if (!visor.Contains(",")) visor += ",";
                    ViewBag.PrimeiroOperador = primeiroValor;
                    ViewBag.Operador = operador;
                    ViewBag.LimpaVisor = "false";
                    break;
                case "+":
                case "-":
                case ":":
                case "*":
                case "=":
                    //processar os operadores
                    //ativar  a 'memoria' do http
                    // -123,8
                    if (operador == null)
                    {
                        ViewBag.PrimeiroValor = visor;
                        ViewBag.Operador = bt;
                        //daV ordem de limpar o visor
                        ViewBag.LimpaVisor = "true";
                    }
                    
                    else
                    {
                        //ja carregou pelo menos uma 2nda vez no sinal de operador
                        double auxprimeiroOperando = Convert.ToDouble(primeiroValor);
                        double auxsegundoOperando = Convert.ToDouble(visor);
                        //executar a operacao
                        switch(operador){
                            case "+" :
                                visor = auxprimeiroOperando + auxsegundoOperando+"";
                            break;
                            case "-":
                                visor = auxprimeiroOperando - auxsegundoOperando + "";
                                break;
                            case "*":
                                visor = auxprimeiroOperando * auxsegundoOperando + "";
                                break;
                            case ":":
                                visor = auxprimeiroOperando / auxsegundoOperando + "";
                                break;
                           

                        }
                        //ativar servico de memoria do http
                        ViewBag.PrimeiroOperador = visor;
                        ViewBag.Operador = bt;
                        //limpar
                        ViewBag.LimpaVisor = "true";
                    }
                    if (bt == "=")
                    {
                        //anular o efeito dos operadores
                        ViewBag.Operador = null;

                    }
                    break;
                case "C" :
                    ViewBag.PrimeiroOperador = null;
                    ViewBag.Operador = null;
                    //limpar
                    ViewBag.LimpaVisor = "true";
                    visor = "0";
                    break;
            
        }//swich
            //exportar dados para a view
            ViewBag.Visor = visor;
           
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
