using System;
using System.Threading.Tasks;
using Coravel.Invocable;
using Zapfenstreich.Services;

namespace Drunkenpolls.Zapfenstreich
{
    public class Zapfenstreich : IInvocable
    {
        private readonly GameService _dbService;

        public Zapfenstreich(GameService dbService)
        {
            _dbService = dbService;

        }

        public Task Invoke()
        {
            Console.WriteLine("Zapfenstreich Invoked.");
            try
            {
                _dbService.GetAllInactive().ForEach(item =>{
                    _dbService.Remove(item.Id);
                });

                return Task.CompletedTask;
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception.Message);
                throw new Exception(exception.Message);
            }

        }
    }

}