using System.Diagnostics.Contracts;

class A
{
  [ContractPublicPropertyName("IsValid")]
  private bool _isValid;
  public bool IsValid {get; private set;}
}