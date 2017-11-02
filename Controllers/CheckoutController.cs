using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Braintree;
using GradedUnitVersion5KeiranDeane.Models;
using GradedUnitVersion5KeiranDeane.ViewModels;
using GradedUnitVersion5.Controllers;
using RestSharp;
using RestSharp.Authenticators;

namespace GradedUnitVersion5.Controllers
{
    public class Constants
    {

        public static BraintreeGateway Gateway = new BraintreeGateway
        {

            Environment = Braintree.Environment.SANDBOX,
            MerchantId = "3c5wr3sv4v6nbxb5",
            PublicKey = "7q63mmxrt4qfp43v",
            PrivateKey = "a1f59d855afa71571da29131cddc6ed6"

        };
    }

    [Authorize]
    public class CheckoutController : Controller
    {

        public ApplicationDbContext db = new ApplicationDbContext();


        public ActionResult AddressAndPayment()
        {
          //  ResponseModel.SendSimpleMessage();
            return View();

        }

        [HttpPost]
        public ActionResult AddressAndPayment(FormCollection values)
        {
            var order = new CustomerOrder();

            
            
                TryUpdateModel(order);



                order.CustomerUserName = User.Identity.Name;
                order.DateCreated = DateTime.Now;

                db.CustomerOrders.Add(order);
                db.SaveChanges();

                var cart = ShoppingCart.GetCart(this.HttpContext);
                cart.CreateOrder(order);

                db.SaveChanges();//we have received the total amount lets update it


                
                if (order.OrderType == "Collection")
                {
                    return RedirectToAction("Complete", new { id = order.Id, email = order.Email });


                }

                else {
                    return RedirectToAction("Payment", new { id = order.Id, email = order.Email });

                }
            }

            
        



        public ActionResult Complete(int id, string email)
        {


            bool isValid = db.CustomerOrders.Any(
                    o => o.Id == id &&
                         o.CustomerUserName == User.Identity.Name
                    );

            if (isValid)
            {


                ResponseModel.SendSimpleMessage(email);
                return View(id);
            }
            else
            {
                var order = new CustomerOrder();
                order = db.CustomerOrders.Find(id);
                db.CustomerOrders.Remove(order);
                return View("Error");
            }
        }



        public ActionResult Payment(int id)
        {

            var order = new CustomerOrder();
            order = db.CustomerOrders.Find(id);
            decimal localAmount = order.Amount;
           string email = order.Email;
            ViewData["TrData"] = Constants.Gateway.Transaction.SaleTrData(
                new TransactionRequest
                {
                    Amount = localAmount,


                    Options = new TransactionOptionsRequest
                    {

                        SubmitForSettlement = true
                    }

                },
                "http://localhost:55134/Result/result"
            );
            ViewData["TransparentRedirectURL"] = Constants.Gateway.TransparentRedirect.Url;
            ResponseModel.SendSimpleMessage(email);
            return View(id);

        }
    }





    public class ResultController : Controller
    {
        public ApplicationDbContext db = new ApplicationDbContext();
        public ActionResult result()
        {
     

            Result<Transaction> result = Constants.Gateway.TransparentRedirect.ConfirmTransaction(Request.Url.Query);
            if (result.IsSuccess())
            {
                Transaction transaction = result.Target;
                ViewData["Message"] = transaction.Status;
                

            }
            else
            {
                ViewData["Message"] = string.Join(", ", result.Errors.DeepAll());
            }

            return View();
        }
    }
}





       public  class ResponseModel
{
    public static IRestResponse SendSimpleMessage(string email)
    {
        var order = new CustomerOrder();
        RestClient client = new RestClient();
        client.BaseUrl = new Uri("https://api.mailgun.net/v3");
        client.Authenticator =
                new HttpBasicAuthenticator("api",
                                           "key-47ce80cdee9d3203fbdd6b0f56b441fc");
        RestRequest request = new RestRequest();
        request.AddParameter("domain",
                             "sandboxee07cf819d0047a2a0678df6cf9ed726.mailgun.org", ParameterType.UrlSegment);
        request.Resource = "{domain}/messages";
        request.AddParameter("from", "Excited User <postmaster@sandboxee07cf819d0047a2a0678df6cf9ed726.mailgun.org>");
        request.AddParameter("to", email);
        request.AddParameter("subject", "Hello");
        request.AddParameter("text", "Testing some Mailgun awesomness!");
        request.Method = Method.POST;
        return client.Execute(request);
    }
}
    




