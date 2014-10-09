program testerepete;
var oi,i:integer;
begin
	oi := 1;
	i := 1;
	repeat
		oi := oi + i;
		i := i + 1;
	until i > 10;
end.