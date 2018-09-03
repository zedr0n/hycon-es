grammar Hycon;

/*
  Parser rules
*/

command : create | putBlock;

create : CREATE ( createBlock );
createBlock : BLOCK hash previousHash;
putBlock : PUT BLOCK hash previousHash;

date : NUMBER NUMBER;
hash : HASH;
previousHash : HASH;
guid : WORD;
guidOptional : newGuid | WORD;
newGuid : NEWLINE;

bagDescriptor : guid | WORD;
assetDescriptor : guid | WORD;

/*
  Lexer Rules
*/

fragment C          : ('C'|'c') ;
fragment R          : ('R'|'r') ;
fragment E          : ('E'|'e') ;
fragment A          : ('A'|'a') ;
fragment B          : ('B'|'b') ;
fragment T          : ('T'|'t') ;
fragment O          : ('O'|'o') ;
fragment I          : ('I'|'i') ;
fragment N          : ('N'|'n') ;
fragment K          : ('K'|'k') ;
fragment W          : ('W'|'w') ;
fragment G          : ('G'|'g') ;
fragment U          : ('U'|'u') ;
fragment D          : ('D'|'d') ;
fragment S          : ('S'|'s') ;
fragment P          : ('P'|'p') ;
fragment M          : ('M'|'m') ;
fragment V          : ('V'|'v') ;
fragment L          : ('L'|'l') ;
fragment Y          : ('Y'|'y') ;
fragment H          : ('H'|'h') ;

fragment LOWERCASE  : [a-z];
fragment UPPERCASE  : [A-Z];
fragment DIGIT     : [0-9];

CREATE : C R E A T E;
BLOCK : B L O C K;
GUID : G U I D;
PUT : P U T;

WORD                : (LOWERCASE | UPPERCASE | '-')+ ;
HASH                : (LOWERCASE | UPPERCASE | DIGIT)+ ;
WHITESPACE          : (' '|'\t')+ -> skip ;
NEWLINE             : ('\r'? '\n' | '\r')+ ;
NUMBER              : DIGIT+;
DOUBLE              : DIGIT+'.'DIGIT+;
