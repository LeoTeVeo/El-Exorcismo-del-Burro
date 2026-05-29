public interface IInteractable
{
    //Este script es una especie de contrato. Todos los objetos que lo tengan están obligados a tener las siguientes funciones declaradas:
    void Interact();
    string GetDescription();
}