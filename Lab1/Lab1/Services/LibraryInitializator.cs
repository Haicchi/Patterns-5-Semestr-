using Lab1.Interfaces;
using Lab1.Model;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Lab1.Services
{
    public class LibraryInitializator
    {
        private readonly ILibraryRepository _repository;
        

        private readonly ILibraryRandomGenerator _randomgenerator;

        public LibraryInitializator(ILibraryRepository repository,ILibraryRandomGenerator randomgenerator)
        {
            _repository = repository;
            
            _randomgenerator = randomgenerator;
        }

        public async Task SeedAsync()
        {
            var existingItems = await _repository.GetAllAsync();
            if (existingItems.Count > 0)
            {
                return;
            }
            for (int i = 0; i < 10; i++)
            {
                await _randomgenerator.GenerateRandomItemAsync();
            }
            

            
    
        }
    }
}