using System.Diagnostics.Contracts;

class A
{
  public object PostconditionWithDerivedType
  {
     get
     {
       // error CC1002: In method CodeContractInvestigations.Postconditions.PostconditionWithDerivedType: Detected a call to Result with 'System.String', should be 'System.Object'.
       {caret}Contract.Ensures(Contract.Result<string>() != null);
       throw new NotImplementedException();
     }
  }
}