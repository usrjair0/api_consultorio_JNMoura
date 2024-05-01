create database consultorio2
use consultorio2;

create table medico (
	id int identity(1,1),
	crm varchar(9),
	nome varchar(100),
	constraint PK_medico primary key (crm)
);

create table Paciente (
	id int identity(1,1),
	nome varchar(100),
	datanascimento date,
	constraint PK_paciente primary key (id)
);

create table medicamento(
	id int identity(1,1),
	nome varchar(100) not null,
	datafabricacao date not null,
	datavencimento date,
	constraint PK_medicamento primary key (id)
);

select id, nome, datafabricacao, datavencimento from medicamento;
insert into medicamento (nome, datafabricacao, datavencimento) values ('Dipirona','1995-08-20', null);
insert into medicamento (nome, datafabricacao, datavencimento) values ('Buscopan2','1999-04-05', '2000-04-05');
insert into medicamento (nome, datafabricacao, datavencimento) values ('Dipirona 2', '2005-07-19', '2050-06-06');

select id, nome, datafabricacao, datavencimento from medicamento where id = 1;