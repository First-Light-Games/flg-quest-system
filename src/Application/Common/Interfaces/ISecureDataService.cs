namespace QuestSystem.Application.Common.Interfaces;

public interface ISecureDataService
{
    string Encrypt<T>(T plainObject);
    T? Decrypt<T>(string cipherText);
}
