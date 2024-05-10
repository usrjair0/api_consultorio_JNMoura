create database consultorio2
use consultorio2;
sp_help medicamento;
create table medico (
	id int identity(1,1),
	crm varchar(9) not null,
	nome varchar(100) not null,
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
insert into medicamento (nome, datafabricacao, datavencimento) values ('Buscopan','1999-04-05', '2000-04-05');
insert into medicamento (nome, datafabricacao, datavencimento) values ('Vitamina C', '2005-07-19', '2010-06-06');

select id, crm, nome from medico;
insert into medico (nome, crm) values ('João Gomes','123456/SP');
insert into medico (nome, crm) values ('Jairo Silva','123457/SP');
insert into medico (nome, crm) values ('Leticia Albuquerque','123458/SP');
insert into medico (nome, crm) values ('Thiago José','123459/RN');


select id, crm, nome from medico where nome like 'j%';