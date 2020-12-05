using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using rpg.Dtos.Character;
using rpg.Models;

namespace rpg.Services.CharacterService
{
  public class CharacterService : ICharacterService
  {
    public IMapper Mapper { get; }

    public CharacterService(IMapper mapper)
    {
      Mapper = mapper;
    }

    private static List<Character> characters = new List<Character> {
        new Character(),
        new Character {Id = 1, Name = "Sam"}
    };

    public async Task<ServiceResponse<List<GetCharacterDto>>> AddCharacter(AddCharacterDto newCharacter)
    {
      var serviceResponse = new ServiceResponse<List<GetCharacterDto>>();
      var character = Mapper.Map<Character>(newCharacter);
      character.Id = characters.Max(character => character.Id) + 1;
      characters.Add(character);
      serviceResponse.Data = (characters.Select(character => Mapper.Map<GetCharacterDto>(character)).ToList());
      return serviceResponse;
    }

    public async Task<ServiceResponse<List<GetCharacterDto>>> GetAllCharacters()
    {
      var serviceResponse = new ServiceResponse<List<GetCharacterDto>>();
      serviceResponse.Data = (characters.Select(character => Mapper.Map<GetCharacterDto>(character)).ToList());
      return serviceResponse;
    }

    public async Task<ServiceResponse<GetCharacterDto>> GetCharacterById(int id)
    {
      var serviceResponse = new ServiceResponse<GetCharacterDto>();
      serviceResponse.Data = Mapper.Map<GetCharacterDto>(characters.FirstOrDefault(character => character.Id == id));
      return serviceResponse;
    }

    public async Task<ServiceResponse<GetCharacterDto>> UpdateCharacter(UpdateCharacterDto updateCharacter)
    {
      var serviceResponse = new ServiceResponse<GetCharacterDto>();
      
      try {
        Character character = characters.FirstOrDefault(character => character.Id == updateCharacter.Id);
        character.Name = updateCharacter.Name;
        character.Class = updateCharacter.Class;
        character.Defense = updateCharacter.Defense;
        character.HitPoints = updateCharacter.HitPoints;
        character.Intelligence = updateCharacter.Intelligence;
        character.Strength = updateCharacter.Strength;
        
        serviceResponse.Data = Mapper.Map<GetCharacterDto>(character);
      } catch(Exception ex) {
        serviceResponse.Success = false;
        serviceResponse.Message = ex.Message;
      }
      
      return serviceResponse;
    }

    public async Task<ServiceResponse<List<GetCharacterDto>>> DeleteCharacter(int characterId)
    {
      var serviceResponse = new ServiceResponse<List<GetCharacterDto>>();
      
      try {
        Character character = characters.First(character => character.Id == characterId);
        characters.Remove(character);
        
        serviceResponse.Data = (characters.Select(character => Mapper.Map<GetCharacterDto>(character)).ToList());
      } catch(Exception ex) {
        serviceResponse.Success = false;
        serviceResponse.Message = ex.Message;
      }
      
      return serviceResponse;
    }
  }
}