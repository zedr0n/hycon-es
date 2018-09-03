grammar Hycon;

/*
  Parser rules
*/

command : create;

create : CREATE ( createBlock );
createBlock : hash previousHash;

date : NUMBER NUMBER;
hash : WORD;
previousHash : WORD;
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
GUID : G U I D;

WORD                : (LOWERCASE | UPPERCASE | '-')+ ;
WHITESPACE          : (' '|'\t')+ -> skip ;
NEWLINE             : ('\r'? '\n' | '\r')+ ;
NUMBER              : DIGIT+;
DOUBLE              : DIGIT+'.'DIGIT+;
