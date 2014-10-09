program tudo;
var oi, i, j, k:integer;
begin
	i := 1;
	oi := 1;
	if oi = 1 then
	begin
		oi := 2;
	end;
	repeat
		oi := oi + 1;
		while i < 10 do
		begin
			oi := i + oi;
			i := i + 1;
		end;
	until oi > 10;
	oi := 0;
	for i := 1 to 10 do
	begin
		oi := i + oi;
		for j := 1 to 10 do
		begin
			oi := i + oi;
			for k := 1 to 10 do
			begin
				oi := i + oi;
			end;
		end;
		for j := 1 to 10 do
		begin
			oi := i + oi;
		end;
	end;
	oi := 0;
	while i < 10 do
	begin
		oi := i + oi;
		i := i + 1;
	end;
end.
