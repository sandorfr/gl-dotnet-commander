# Juan - An Operation Manager Framework

As the Commander (developer), you planned a few set of operations. You expect them
to run smoothly when you're not there (outside of your environement). 
But this was without counting Juan (user). This charimastic pain in this ass
will mess with everything you planned, he will push everyone to the edge
if possible at the multiple times and concurrently. eventually, Elvira, 
you beloved daugther will come back to you after an emotional crash (report). 

This Framework is all about providing you an easy fluent api, to define operations in 
your code (for instance viewmodels), and avoid Juan messing with you once and for all 
so you may rest in peace !

If you don't get this introduction, pleas read the `Get Started` or
read `Dom Juan` from the french write `Molière` or rush on your next
local Opera playing of `Don Gionvanni` from the infamous `Mozart`.

## Get Started

Let's say you have a `ViewModel` for a quote wich exposes to methods 
one to delete the quote one to place the order.
```csharp
public Task Order();
public Task Delete();
```

It's quite common sense that both operations cannot be executed at the
same time. With tradionnal programming you may start adding booleans, 
binding to them and so on. But this create quite a lot of boilerplate
each time you need it (event with tradionnal commands things can get messy).

With Commander, you may write the following :
```csharp

public QuoteViewModel() {

	this.Order = Operation.For(OnOrder);
	this.Delete = Operation.For(OnDelete).Exclusive(this.Order);
}

public IOperation Order { get; }
public IOperation Delete { get; }

private Task OnOrder();
private Task OnDelete();
```

With the following code, `Delete` won't be callable while Order 
is processing. 

## Features

* Fire and Forget tooling to prevent friendly fire and back fire.
* Cancellability support
* Exclusive execution: One way exclusive execution
* Exclusivity sets [Coming soon] : easily implement mutually exclusive set of operation
* Polly support [Coming soon]
* `ICommand` bridge
* Xamarin.IOS operation linker [Coming soon] (so you can use us with or without an MVVM Framework)
* Xamarin.Android operation linker [Coming soon]