parser grammar ShaellParser;

options {
    tokenVocab = 'ShaellLexer';
}

prog: programArgs stmts | stmts;
stmts: stmt*;
stmt: ifStmt 
    | forLoop 
    | whileLoop 
    | returnStatement 
    | functionDefinition 
    | foreach 
    | foreachKeyValue 
    | throwStatement
    | pipeProgram
    | expr
    ;
boolean: 
    TRUE # TrueBoolean 
    | FALSE # FalseBoolean
    ;
expr: 
    //unsorted
    DQUOTE strcontent* END_STRING # StringLiteralExpr
    | LET IDENTIFIER # LetExpr
    | NUMBER # NumberExpr
    | NULL # NullExpr
    | boolean # BooleanExpr
    | TRY stmts END #TryExpr
    | IDENTIFIER # IdentifierExpr
    | LPAREN expr RPAREN # Parenthesis
    | LCURL (objfields ASSIGN expr (COMMA objfields ASSIGN expr)*)? RCURL #ObjectLiteral
    | progProgram #ProgProgramExpr
    //sorted
    |<assoc=right> DEREF expr # DerefExpr
    | expr COLON IDENTIFIER # IdentifierIndexExpr
    | expr LSQUACKET expr RSQUACKET # SubScriptExpr
    | expr LPAREN innerArgList RPAREN # FunctionCallExpr
    |<assoc=right> LNOT expr # LnotExpr
    |<assoc=right> MINUS expr # NegExpr
    |<assoc=right> PLUS expr # PosExpr
    | expr POW expr # PowExpr
    | expr MULT expr # MultExpr
    | expr DIV expr # DivExpr
    | expr MOD expr # ModExpr
    | expr PLUS expr # AddExpr
    | expr MINUS expr # MinusExpr
    | expr LT expr # LTExpr
    | expr LEQ expr # LEQExpr
    | expr GT expr # GTExpr
    | expr GEQ expr # GEQExpr
    | expr EQ expr # EQExpr
    | expr NEQ expr # NEQExpr
    | expr LAND expr # LANDExpr
    | expr LOR expr # LORExpr
    |<assoc=right> expr ASSIGN expr # AssignExpr
    |<assoc=right> expr PLUSEQ expr  # PlusEqExpr
    |<assoc=right> expr MINUSEQ expr # MinusEqExpr
    |<assoc=right> expr MULTEQ expr # MultEqExpr
    |<assoc=right> expr DIVEQ expr # DivEqExpr
    |<assoc=right> expr MODEQ expr # ModEqExpr
    |<assoc=right> expr POWEQ expr # PowEqExpr
    |anonFunctionDefinition # AnonFnDefinition
	;
pipeTarget: progProgram | pipeProgram;

progProgram:
    PROG expr (WITH LPAREN innerArgList RPAREN)? (INTO (pipeTarget | expr))?;
pipeProgram:
    PIPE expr (WITH LPAREN innerArgList RPAREN)? pipeDesc* END;
pipeDesc:
    IDENTIFIER INTO pipeTarget #IntoDesc
    |IDENTIFIER INTO expr #OntoDesc
    ;
    
strcontent:
    NEWLINE # NewLine
    | ESCAPEDINTERPOLATION #EscapedInterpolation
    | ESCAPEDESCAPE #EscapedEscape
    | INTERPOLATION expr STRINGCLOSEBRACE # Interpolation
    | TEXT # StringLiteral
    ;
objfields:
    IDENTIFIER # FieldIdentifier
    | LSQUACKET expr RSQUACKET #FieldExpr
    ;
innerArgList: (expr (COMMA expr)*)?;
innerFormalArgList: (IDENTIFIER (COMMA IDENTIFIER)*)?;

programArgs: ARGS IDENTIFIER innerFormalArgList END;

ifStmt: IF expr THEN stmts (ELSE stmts)? END;
forLoop: FOR expr COMMA expr COMMA expr DO stmts END;
foreach: FOREACH IDENTIFIER IN expr DO stmts END;
foreachKeyValue: FOREACH IDENTIFIER COMMA IDENTIFIER IN expr DO stmts END;
whileLoop: WHILE expr DO stmts END;
functionDefinition: FUNCTION IDENTIFIER LPAREN innerFormalArgList RPAREN functionBody;
anonFunctionDefinition: FUNCTION LPAREN innerFormalArgList RPAREN functionBody;
functionBody: stmts END
    | LAMBDA expr
    ;
returnStatement: RETURN expr;
throwStatement: THROW expr; 
