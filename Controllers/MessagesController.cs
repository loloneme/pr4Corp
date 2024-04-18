using System.Diagnostics;
using pr4.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace pr4.Controllers;


public class MessagesController : Controller
{
    ApplicationContext db;
    public MessagesController(ApplicationContext context)
    {
        db = context;
    }

    public IActionResult ShowMessages(string login)
    {
        
        // var msgList = db.Message.Where(x => x.To == login || x.From == login).OrderBy(x => x.SentDate);
        // ViewBag.Senders = msgList.Select(x => x.From).Distinct().ToList();
        // ViewBag.Senders.Insert(0, "Все");
        // ViewBag.UserInfo = db.User.Where(x => x.Login == login).Select(x => new {Name = x.Surname + " " + x.Name + " " + x.Patronymic}).First();

        var ViewModel = new ShowMessagesViewModel{
            // Messages = db.Message.Where(x => x.To == login || x.From == login).OrderBy(x => x.SentDate),
            Filters = new FiltersModel{
                    StatusFilter = "Все",
                    // StartDate
                    // EndDate
                    SenderFilter = "Все"
            },
            Login = login
        };
        return FilterMessages(ViewModel);
        
        // return View(ViewModel);
    }

    public IActionResult ShowMessage(int id, string login)
    {
        SetReadStatus(id, login);
        
        return View(db.Message.Find(id));
    }

    public IActionResult SetReadStatusWithRedirect(int id, string login){
        SetReadStatus(id, login);

        return RedirectToAction("ShowMessages", new { login });
    }

    public void SetReadStatus(int id, string login)
    {
        var msg = db.Message.Find(id);
        if (msg.From != login) {
            msg.Status = "Прочитано";
            db.SaveChanges();
        }
        // return RedirectToAction("ShowMessages", new { login });
    }

    //     public IActionResult SendMessage(){
    //         ViewBag.From = Request.Query["login"];
    // // 
    //         return View();
    //     }

    public IActionResult SendMessage(Message msg)
    {
        if (msg.To != null)
        {
            bool userExists = db.User.Any(x => x.Login == msg.To);
            if (!userExists)
            {
                ViewBag.ErrorMessage = "Данного адресата не существует";
                return View();
            }

            db.Add(msg);
            db.SaveChangesAsync();
            return RedirectToAction("MessageSent", new { to = msg.To, login = msg.From });

        } 
        return View();
    }

    public IActionResult MessageSent(string to, string login)
    {
        ViewBag.To = to;
        ViewBag.Login = login;
        return View();
    }

    private IEnumerable<Message> FilterByTypeOfDispatch(string login, string status){
        IEnumerable<Message> msgList;

        if (status == "Все"){
            msgList = db.Message.Where(x => x.To == login || x.From == login);
        } else if (status == "Отправленные"){
           msgList = db.Message.Where(x => x.From == login);
        } else {
           msgList = db.Message.Where(x => x.To == login);
        }

        return msgList;
    }
        
    private IEnumerable<Message> FilterByStatus(IEnumerable<Message> msgList, string status){
        if (status == "Прочитанные"){
            msgList = msgList.Where(x => x.Status == "Прочитано");
        } else if (status == "Непрочитанные"){
            msgList = msgList.Where(x => x.Status == "Не прочитано");
        }
        return msgList;
    }

    IEnumerable<Message> FilterBySender(IEnumerable<Message> msgList, string status){
        if (status != "Все"){
            msgList = msgList.Where(x => x.From == status);
        }
        return msgList;
    }

    IEnumerable<Message> FilterByDate(IEnumerable<Message> msgList, DateTimeOffset startDate, DateTimeOffset endDate){
        if (startDate != DateTimeOffset.MinValue && endDate != DateTimeOffset.MinValue){
            msgList = msgList.Where(x => x.SentDate >= startDate && x.SentDate <= endDate);
        } else if (startDate != DateTimeOffset.MinValue){
            msgList = msgList.Where(x => x.SentDate >= startDate);
        } else if (endDate != DateTimeOffset.MinValue){
            msgList = msgList.Where(x => x.SentDate <= endDate);
        }

        return msgList;
    }

    public IActionResult FilterMessages(ShowMessagesViewModel viewModel){
        var msgList = db.Message.Where(x => x.To == viewModel.Login || x.From == viewModel.Login);

        ViewBag.Senders = msgList.Select(x => x.From).Distinct().ToList();
        ViewBag.Senders.Insert(0, "Все");
        ViewBag.UserInfo = db.User.Where(x => x.Login == viewModel.Login).Select(x => new {Name = x.Surname + " " + x.Name + " " + x.Patronymic}).First();

        viewModel.Messages = FilterByStatus(msgList, viewModel.Filters.StatusFilter);
        viewModel.Messages = FilterBySender(viewModel.Messages, viewModel.Filters.SenderFilter);
        viewModel.Messages = FilterByDate(viewModel.Messages, viewModel.Filters.StartDate, viewModel.Filters.EndDate);
        viewModel.Messages.OrderBy(x => x.SentDate);

        return View("ShowMessages", viewModel);
    }

    // public IActionResult FilterMessagesBySender(ShowMessagesViewModel viewModel)
    // {
    //     string SelectedStatus = viewModel.SelectedStatus;


    //     viewModel.Messages = msgList;

    //     return View("ShowMessages", viewModel);
    // }


    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
